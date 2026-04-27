// Main JavaScript for Consultancy Website

// Language switcher
document.addEventListener('DOMContentLoaded', function() {
    const urlParams = new URLSearchParams(window.location.search);
    const lang = urlParams.get('lang');
    if (lang) {
        fetch(`?handler=SetLanguage&lang=${lang}`, {
            method: 'POST',
            headers: {
                'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]')?.value
            }
        }).then(() => {
            window.location.href = window.location.pathname;
        });
    }
});

// Mobile menu toggle
const menuButton = document.querySelector('[data-menu-toggle]');
const mobileMenu = document.getElementById('mobile-menu');

if (menuButton && mobileMenu) {
    menuButton.addEventListener('click', () => {
        mobileMenu.classList.toggle('hidden');
    });
}

// Smooth scroll
document.querySelectorAll('a[href^="#"]').forEach(anchor => {
    anchor.addEventListener('click', function (e) {
        e.preventDefault();
        const target = document.querySelector(this.getAttribute('href'));
        if (target) {
            target.scrollIntoView({
                behavior: 'smooth'
            });
        }
    });
});

// Contact form submission
const contactForm = document.getElementById('contact-form');
if (contactForm) {
    contactForm.addEventListener('submit', async (e) => {
        e.preventDefault();
        const formData = new FormData(contactForm);
        
        try {
            const response = await fetch('/contact?handler=Submit', {
                method: 'POST',
                body: formData,
                headers: {
                    'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]')?.value
                }
            });
            
            if (response.ok) {
                alert('Thank you! Your message has been sent.');
                contactForm.reset();
            }
        } catch (error) {
            console.error('Error:', error);
        }
    });
}

// Student registration form
const studentForm = document.getElementById('student-form');
if (studentForm) {
    studentForm.addEventListener('submit', async (e) => {
        e.preventDefault();
        const formData = new FormData(studentForm);
        
        try {
            const response = await fetch('/register?handler=Submit', {
                method: 'POST',
                body: formData,
                headers: {
                    'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]')?.value
                }
            });
            
            if (response.ok) {
                alert('Thank you for registering! We will contact you soon.');
                studentForm.reset();
            }
        } catch (error) {
            console.error('Error:', error);
        }
    });
}