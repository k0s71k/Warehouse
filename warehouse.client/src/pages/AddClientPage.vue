<template>
  <div class="card shadow-sm">
    <div class="card-body align-items-center">
      <form class="w-100 py-3" @submit.prevent="addClient">
        <span class="fs-3 text-center">Добавление клиента</span>
        <div class="col">
          <input class="form-control w-100 mt-3" v-model="client.name" placeholder="Введите название" />
          <input class="form-control w-100 mt-3" v-model="client.address" placeholder="Введите адрес" />
          <button class="btn btn-success w-100 mt-3" type="submit">
            Добавить
          </button>
        </div>
      </form>
    </div>
  </div>
</template>

<script setup>
  import { ref } from 'vue';
  import axios from 'axios';
  import router from '@/router';

  const client = ref({
    name: '',
    address: ''
  });

  const addClient = async () => {
    try {
      await axios.post('/api/client/add', client.value);
      router.push('/clients');
    } catch (ex) {
      alert(ex.response?.data || "Произошла ошибка");
    }
  };
</script>
