let darkMode = false;

function toggleDarkMode()
{
    darkMode = !darkMode;
    document.documentElement.classList.toggle('dark', darkMode);
    localStorage.setItem('darkMode', darkMode);
}

function loadDarkMode()
{
    const saved = localStorage.getItem('darkMode');
    if (saved !== null)
    {
        darkMode = saved === 'true';
    }
    else
    {
        darkMode = window.matchMedia('(prefers-color-scheme: dark)').matches;
    }
    document.documentElement.classList.toggle('dark', darkMode);
}

function toggleMobileMenu()
{
    const menu = document.getElementById('mobileMenu');
    const overlay = document.getElementById('mobileOverlay');
    if (menu && overlay)
    {
        menu.classList.toggle('open');
        overlay.classList.toggle('open');
    }
}

function closeMobileMenu()
{
    const menu = document.getElementById('mobileMenu');
    const overlay = document.getElementById('mobileOverlay');
    if (menu && overlay)
    {
        menu.classList.remove('open');
        overlay.classList.remove('open');
    }
}

function updateTime()
{
    const now = new Date();
    const options = { weekday: 'long', year: 'numeric', month: 'long', day: 'numeric', hour: '2-digit', minute: '2-digit' };
    const timeElement = document.getElementById('currentTime');
    if (timeElement)
    {
        timeElement.textContent = now.toLocaleDateString('en-US', options);
    }
}

function handleScroll()
{
    const scrollToTop = document.getElementById('scrollToTop');
    if (scrollToTop)
    {
        if (window.pageYOffset > 300)
        {
            scrollToTop.classList.add('visible');
        }
        else
        {
            scrollToTop.classList.remove('visible');
        }
    }
}

function scrollToTop()
{
    window.scrollTo({ top: 0, behavior: 'smooth' });
}

function showToast(message, type = 'info')
{
    const existingToast = document.querySelector('.toast-message');
    if (existingToast)
    {
        existingToast.remove();
    }

    const toast = document.createElement('div');
    toast.className = `toast-message fixed top-4 right-4 px-6 py-4 rounded-lg shadow-lg z-50 toast ${type === 'success' ? 'bg-green-500' : type === 'error' ? 'bg-red-500' : 'bg-blue-500'} text-white`;
    toast.textContent = message;
    document.body.appendChild(toast);

    setTimeout(() => {
        toast.style.animation = 'toastSlide 0.3s ease-in reverse';
        setTimeout(() => toast.remove(), 300);
    }, 3000);
}

function copyToClipboard(text)
{
    navigator.clipboard.writeText(text).then(() => {
        showToast('Copied to clipboard!', 'success');
    }).catch(() => {
        showToast('Failed to copy', 'error');
    });
}

function animateOnScroll()
{
    const elements = document.querySelectorAll('.fade-in, .slide-in');
    const observer = new IntersectionObserver((entries) => {
        entries.forEach(entry => {
            if (entry.isIntersecting)
            {
                entry.target.style.animationPlayState = 'running';
            }
        });
    }, { threshold: 0.1 });

    elements.forEach(el => {
        el.style.animationPlayState = 'paused';
        observer.observe(el);
    });
}

function formatNumber(num)
{
    if (num >= 1000000)
    {
        return (num / 1000000).toFixed(1) + 'M';
    }
    if (num >= 1000)
    {
        return (num / 1000).toFixed(1) + 'K';
    }
    return num.toString();
}

function formatBytes(bytes)
{
    if (bytes === 0)
        return '0 Bytes';
    const k = 1024;
    const sizes = ['Bytes', 'KB', 'MB', 'GB'];
    const i = Math.floor(Math.log(bytes) / Math.log(k));
    return parseFloat((bytes / Math.pow(k, i)).toFixed(2)) + ' ' + sizes[i];
}

function debounce(func, wait)
{
    let timeout;
    return function executedFunction(...args)
    {
        const later = () => {
            clearTimeout(timeout);
            func(...args);
        };
        clearTimeout(timeout);
        timeout = setTimeout(later, wait);
    };
}

document.addEventListener('DOMContentLoaded', () => {
    loadDarkMode();
    updateTime();
    setInterval(updateTime, 60000);
    animateOnScroll();
    
    window.addEventListener('scroll', handleScroll);
    window.addEventListener('resize', debounce(() => {
        if (window.innerWidth >= 768)
        {
            closeMobileMenu();
        }
    }, 250));
});

window.addEventListener('beforeunload', () => {
    localStorage.setItem('scrollPosition', window.pageYOffset);
});

window.addEventListener('load', () => {
    const scrollPosition = localStorage.getItem('scrollPosition');
    if (scrollPosition)
    {
        window.scrollTo(0, parseInt(scrollPosition));
        localStorage.removeItem('scrollPosition');
    }
});
