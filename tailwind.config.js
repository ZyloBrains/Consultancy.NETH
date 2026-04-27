module.exports = {
  content: [
    "./Areas/**/*.cshtml",
    "./Pages/**/*.cshtml",
    "./Shared/**/*.cshtml"
  ],
  theme: {
    extend: {
      colors: {
        primary: {
          DEFAULT: '#004aad',
          dark: '#003380',
          light: '#1a65d4'
        },
        secondary: '#f4c430',
        accent: '#f4c430',
        dark: '#1f2937',
        light: '#f3f4f6'
      },
      container: {
        center: true,
        padding: '1rem'
      }
    },
  },
  plugins: [],
}