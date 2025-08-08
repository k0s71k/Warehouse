<template>
  <div v-if="loading" class="text-center py-5">
    <div class="spinner-border text-primary" role="status"></div>
  </div>
  <div v-else>
    <div class="card shadow-sm">
      <div class="card-body col">
        <span class="fs-3">Список ед. измерения</span>
        <div class="row mt-3">
          <div class="col-md-4">
            <button class="btn btn-success w-100" @click="addUserClick">
              Добавить
            </button>
          </div>
          <div class="col-md-4">
            <button class="btn btn-secondary w-100" @click="archievedClick">
              {{ showArchievedText }}
            </button>
          </div>
          <div class="col-md-4">
            <button class="btn btn-primary w-100" @click="applyClick">
              Применить
            </button>
          </div>
        </div>
        <table class="table text-center">
          <thead>
            <tr>
              <th></th>
              <th>Название</th>
              <th>В архиве</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="measureUnit in measureUnits" :key="measureUnit.guid">
              <td><button @click="deleteMeasureUnit(measureUnit.guid)" class="btn btn-danger">Х</button></td>
              <td><input class="form-control w-100" v-model="measureUnit.name" /></td>
              <td><input type="checkbox" class="w-100 btn form-check" v-model="measureUnit.isArchieved" /></td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>
  </div>
</template>

<script setup>
  import { onMounted, ref, computed } from 'vue';
  import axios from 'axios';
  import router from '@/router';

  const loading = ref(true);
  const measureUnits = ref([]);
  const showArchieved = ref(false);

  const showArchievedText = computed(() =>
    showArchieved.value ? "Показать активные ед. измерения" : "Показать архив ед. измерения");

  const addUserClick = () => {
    router.push('/measure-units/add');
  }
  const archievedClick = async () => {
    showArchieved.value = !showArchieved.value;
    await fetchMeasureUnits();
  }
  const applyClick = async () => {
    try {
      await axios.put('/api/measure-unit/update', measureUnits.value);
      await fetchMeasureUnits();
    } catch (ex) {
      alert(ex.response?.data || "Произошла ошибка");
    }
  }

  const fetchMeasureUnits = async () => {
    try {
      const response = await axios.get(
        showArchieved.value ? '/api/measure-unit/archieved' : '/api/measure-unit/active');

      measureUnits.value = response.data;
    } catch (ex) {
      alert(ex.response?.data || "Произошла ошибка");
    }
  }

  const deleteMeasureUnit = async (id) => {
    try {
      await axios.delete(`/api/measure-unit/${id}`)
      await fetchMeasureUnits();
    } catch (ex) {
      alert(ex.response?.data || "Произошла ошибка");
    }
  }

  onMounted(async () => {
    loading.value = true;
    await fetchMeasureUnits();
    loading.value = false;
  })
</script>
