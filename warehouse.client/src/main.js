import '@/assets/main.css'
import 'bootstrap'
import 'bootstrap/dist/css/bootstrap.css'

import { createApp } from 'vue'
import App from '@/App.vue'
import router from '@/router.js'

import axios from 'axios'
if (import.meta.env.DEV === true) {
  axios.defaults.baseURL = "http://localhost:5062"
}

const app = createApp(App)

app.use(router)
app.mount("#app")
