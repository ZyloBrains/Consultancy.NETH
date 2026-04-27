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
          DEFAULT: '#2563eb',
          dark: '#1d4ed8',
          light: '#3b82f6'
        },
        secondary: {
          DEFAULT: '#0f766e',
          dark: '#0d655e'
        },
        accent: '#f59e0b',
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