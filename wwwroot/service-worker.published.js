// � ���� ����� �� ��������� ����������� � ��������� �������� ��� ��������� �������-������.

self.importScripts('./service-worker-assets.js');

self.addEventListener('install', event => {
    console.info('Service worker: Install');
    event.waitUntil(onInstall(event));
});

self.addEventListener('activate', event => {
    console.info('Service worker: Activate');
    event.waitUntil(onActivate(event));
});

// ��������� �����������
const cacheNamePrefix = 'offline-cache-';
const cacheName = `${cacheNamePrefix}${self.assetsManifest.version}`;
const offlineAssetsInclude = [/\.dll$/, /\.pdb$/, /\.wasm/, /\.html/, /\.js$/, /\.json$/, /\.css$/, /\.woff$/, /\.png$/, /\.jpe?g$/, /\.gif$/, /\.ico$/, /\.blat$/, /\.dat$/];
const offlineAssetsExclude = [/^service-worker\.js$/];

// ������� ����
const base = "/";
const baseUrl = new URL(base, self.origin);
const manifestUrlList = self.assetsManifest.assets.map(asset => new URL(asset.url, baseUrl).href);

// ��������� ���������
async function onInstall(event) {
    const assetsRequests = self.assetsManifest.assets
        .filter(asset => offlineAssetsInclude.some(pattern => pattern.test(asset.url)))
        .filter(asset => !offlineAssetsExclude.some(pattern => pattern.test(asset.url)))
        .map(asset => new Request(asset.url, { integrity: asset.hash, cache: 'no-cache' }));

    const cache = await caches.open(cacheName);
    await cache.addAll(assetsRequests);
}

// ��������� ���������
async function onActivate(event) {
    const cacheKeys = await caches.keys();
    await Promise.all(cacheKeys
        .filter(key => key.startsWith(cacheNamePrefix) && key !== cacheName)
        .map(key => caches.delete(key)));
}

// ��������� ������� ��������
self.addEventListener('fetch', event => {
    event.respondWith(
        caches.match(event.request)
            .then(response => {
                // ���� � ���� ���� �����, ���������� ���
                if (response) {
                    return response;
                }
                // ���� ���, ��������� ������� ������
                return fetch(event.request).then(networkResponse => {
                    // �������� ����� ��� ������������ �������������
                    return caches.open(cacheName).then(cache => {
                        cache.put(event.request, networkResponse.clone());
                        return networkResponse;
                    });
                });
            })
            .catch(() => {
                // � ������ ������ ���������� "�� �������"
                return new Response("404 Not Found", { status: 404 });
            })
    );
});
