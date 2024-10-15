/** @type {import('tailwindcss').Config} */
module.exports = {
    content: [
        './Components/**/*.razor',
    ],
    theme: {
        extend: {
            colors: {
                'primary': '#19325d',
                'secondary': '#f47424',
                'danger': '#e3342f',
                'success': '#38c172',
                'info': '#3490dc',
                'warning': '#fceb28',
            }
        },
    },
    plugins: [],
}

