window.toggleTheme = (isDarkTheme) => {
    const darkThemeLink = document.getElementById("dark-theme");
    const lightThemeLink = document.getElementById("light-theme");

    if (isDarkTheme) {
        darkThemeLink.removeAttribute("disabled");
        lightThemeLink.setAttribute("disabled", "true");
    } else {
        lightThemeLink.removeAttribute("disabled");
        darkThemeLink.setAttribute("disabled", "true");
    }

    // Сохраните текущую тему в localStorage
    const theme = isDarkTheme ? 'dark' : 'light'; // Установка переменной theme
    localStorage.setItem('theme', theme);
};
