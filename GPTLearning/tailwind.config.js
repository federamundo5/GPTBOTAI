module.exports = {
    content: [
        "./Views/**/*.cshtml",
        "./wwwroot/js/**/*.js" // Add this if you're using JS for dynamic class names
    ],
    theme: {
        extend: {
            colors: {
                primary: "#727D73",  // Dark Greenish
                secondary: "#AAB99A", // Soft Green
                light: "#D0DDD0",  // Light Background
                accent: "#F0F0D7"   // Soft Yellowish
            }
        }
    },
    plugins: [
        require('tailwind-scrollbar'), // Enable the scrollbar plugin
    ],
}

module.exports = {
    theme: {
        extend: {
            fontFamily: {
                robotic: ['Orbitron', 'sans-serif'],
            }
        }
    }
}
