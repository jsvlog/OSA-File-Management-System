function toggleMobileMenu() {
    const menu = document.getElementById('mobileMenu');
    if (menu) {
        menu.classList.toggle('hidden');
    }
}

function showToast(message, type) {
    type = type || 'info';
    var existingToast = document.querySelector('.toast-message');
    if (existingToast) {
        existingToast.remove();
    }

    var toast = document.createElement('div');
    var bgColor = type === 'success' ? 'bg-green-500' : type === 'error' ? 'bg-red-500' : 'bg-blue-500';
    toast.className = 'toast-message fixed top-4 right-4 px-6 py-4 rounded-lg shadow-lg z-50 toast ' + bgColor + ' text-white';
    toast.textContent = message;
    document.body.appendChild(toast);

    setTimeout(function () {
        toast.remove();
    }, 3000);
}

document.addEventListener('DOMContentLoaded', function () {
    // Auto-close mobile menu on link click
    var mobileLinks = document.querySelectorAll('#mobileMenu a');
    mobileLinks.forEach(function (link) {
        link.addEventListener('click', function () {
            var menu = document.getElementById('mobileMenu');
            if (menu) {
                menu.classList.add('hidden');
            }
        });
    });
});