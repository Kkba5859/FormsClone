// В этом файле мы добавляем кэширование и обработку запросов для поддержки оффлайн-режима.

self.importScripts('./service-worker-assets.js');

self.addEventListener('install', event => {
    console.info('Service worker: Install');
    event.waitUntil(onInstall(event));
});

self.addEventListener('activate', event => {
    console.info('Service worker: Activate');
    event.waitUntil(onActivate(event));
});

// Настройки кэширования
const cacheNamePrefix = 'offline-cache-';
const cacheName = `${cacheNamePrefix}${self.assetsManifest.version}`;
const offlineAssetsInclude = [/\.dll$/, /\.pdb$/, /\.wasm/, /\.html/, /\.js$/, /\.json$/, /\.css$/, /\.woff$/, /\.png$/, /\.jpe?g$/, /\.gif$/, /\.ico$/, /\.blat$/, /\.dat$/];
const offlineAssetsExclude = [/^service-worker\.js$/];

// Базовый путь
const base = "/";
const baseUrl = new URL(base, self.origin);
const manifestUrlList = self.assetsManifest.assets.map(asset => new URL(asset.url, baseUrl).href);

// Обработка установки
async function onInstall(event) {
    const assetsRequests = self.assetsManifest.assets
        .filter(asset => offlineAssetsInclude.some(pattern => pattern.test(asset.url)))
        .filter(asset => !offlineAssetsExclude.some(pattern => pattern.test(asset.url)))
        .map(asset => new Request(asset.url, { integrity: asset.hash, cache: 'no-cache' }));

    const cache = await caches.open(cacheName);
    await cache.addAll(assetsRequests);
}

// Обработка активации
async function onActivate(event) {
    const cacheKeys = await caches.keys();
    await Promise.all(cacheKeys
        .filter(key => key.startsWith(cacheNamePrefix) && key !== cacheName)
        .map(key => caches.delete(key)));
}

// Обработка сетевых запросов
self.addEventListener('fetch', event => {
    event.respondWith(
        caches.match(event.request)
            .then(response => {
                // Если в кэше есть ответ, используем его
                if (response) {
                    return response;
                }
                // Если нет, выполняем сетевой запрос
                return fetch(event.request).then(networkResponse => {
                    // Кэшируем ответ для последующего использования
                    return caches.open(cacheName).then(cache => {
                        cache.put(event.request, networkResponse.clone());
                        return networkResponse;
                    });
                });
            })
            .catch(() => {
                // В случае ошибки возвращаем "не найдено"
                return new Response("404 Not Found", { status: 404 });
            })
    );
});
