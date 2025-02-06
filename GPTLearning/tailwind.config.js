module.exports = {
    content: [
        "./Views/**/*.cshtml",
        "./wwwroot/js/**/*.js"
    ],
    theme: {
        extend: {
            colors: {
                primary: "#727D73",  // Dark Greenish
                secondary: "#AAB99A", // Soft Green
                light: "#D0DDD0",  // Light Background
                accent: "#F0F0D7"   // Soft Yellowish
            },
            fontFamily: {
                robotic: ['Orbitron', 'sans-serif']
            }
        }
    },
    corePlugins: {
        animation: true,
    },
    important: true, // Add this line to apply !important to all Tailwind classes
}
