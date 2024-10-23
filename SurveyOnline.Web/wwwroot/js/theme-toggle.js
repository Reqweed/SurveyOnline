const cloud = document.getElementById('word-cloud');
const savedTheme = localStorage.getItem('theme') || 'light';

document.documentElement.setAttribute('data-bs-theme', savedTheme);

applyThemeToCloud(savedTheme);

document.getElementById("themeToggle").addEventListener("click", function () {
    const currentTheme = localStorage.getItem('theme') === 'dark' ? 'light' : 'dark';

    document.documentElement.setAttribute('data-bs-theme', currentTheme);
    localStorage.setItem('theme', currentTheme);

    applyThemeToCloud(currentTheme);
});

function  applyThemeToCloud(theme){
    if (cloud) {
        cloud.style.backgroundColor = theme === 'dark' ? '#212529' : 'rgb(255, 255, 255)';
    }
}