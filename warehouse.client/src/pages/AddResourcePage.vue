<template>
  <div class="card shadow-sm">
    <div class="card-body align-items-center">
      <form class="w-100 py-3" @submit.prevent="addResource">
        <span class="fs-3 text-center">Добавление ресурса</span>
        <div class="col">
          <input class="form-control w-100 mt-3" v-model="resourceName" placeholder="Введите название" />
          <button class="btn btn-success w-100 mt-3" type="submit">Добавить</button>
        </div>
      </form>
    </div>
  </div>
</template>

<script setup>
  import { ref } from 'vue';
  import axios from 'axios';
  import router from '@/router';

  const resourceName = ref('');

  const addResource = async () => {
    try {
      await axios.post(`/api/resource/add/${resourceName.value}`);
      router.push('/resources');
    } catch (ex) {
      alert(ex.response?.data || "Произошла ошибка");
    }
  };
</script>
