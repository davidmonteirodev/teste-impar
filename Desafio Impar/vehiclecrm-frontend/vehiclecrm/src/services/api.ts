import axios from 'axios'

const BASE_URL: string = import.meta.env.VITE_API_URL ?? 'http://localhost:5106/api'

const api = axios.create({
  baseURL: BASE_URL.replace(/\/+$/, '') + '/',
  headers: {
    'Content-Type': 'application/json',
  },
})

export default api
