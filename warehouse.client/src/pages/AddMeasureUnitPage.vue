<template>
  <div class="card shadow-sm">
    <div class="card-body align-items-center">
      <form class="w-100 py-3" @submit.prevent="addMeasureUnit">
        <span class="fs-3 text-center">Добавление единицы измерения</span>
        <div class="col">
          <input class="form-control w-100 mt-3" v-model="measureUnitName" placeholder="Введите название" />
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

  const measureUnitName = ref('');

  const addMeasureUnit = async () => {
    try {
      await axios.post(`/api/measure-unit/add/${measureUnitName.value}`);
      router.push('/measure-units');
    } catch (ex) {
      alert(ex.response?.data || "Произошла ошибка");
    }
  };
</script>
