<template>
  <div v-if="loading" class="text-center">
    <div class="spinner-border text-primary" role="status"></div>
  </div>
  <div class="card shadow-sm p-3" v-else>
    <span class="fs-3">Текущий баланс ресурсов</span>
    <form class="my-3" @submit.prevent="fetchBalances()">
      <div class="row">
        <div class="col-md-4">
          <label class="d-inline-block" for="resource">Поиск по ресурсу:</label>
          <select id="resource" v-model="filter.resources" class="form-select mt-1" multiple>
            <option selected v-for="resource in availableResources" :key="resource.guid" :value="resource">
              {{ resource.name }}
            </option>
          </select>
        </div>
        <div class="col-md-4">
          <label class="d-inline-block" for="measureUnit">Поиск по единице измерения:</label>
          <select id="measureUnit" v-model="filter.measureUnits" class="form-select mt-1" multiple>
            <option selected v-for="measureUnit in availableMeasureUnits" :key="measureUnit.guid" :value="measureUnit">
              {{ measureUnit.name }}
            </option>
          </select>
        </div>
        <div class="col-md-4 align-bottom">
          <button type="submit" class="btn btn-primary w-100">Применить</button>
        </div>
      </div>
    </form>
    <table class="table table-bordered">
      <thead>
        <tr>
          <th>Ресурс</th>
          <th>Ед. измерения</th>
          <th>Количество</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="balance in balances" :key="balance.guid">
          <td>{{ balance.resource.name }}</td>
          <td>{{ balance.measureUnit.name }}</td>
          <td>{{ balance.quantity }}</td>
        </tr>
      </tbody>
    </table>
  </div>
</template>

<script setup>
  import { ref, onMounted } from 'vue';
  import axios from 'axios';

  const loading = ref(true);
  const filter = ref({
    resources: [],
    measureUnits: [],
  });

  const balances = ref([]);
  const availableResources = ref([]);
  const availableMeasureUnits = ref([]);

  const fetchBalances = async () => {
    try {
      const response = await axios.post(
        '/api/balance/filter',
        filter.value);

      balances.value = response.data;
    } catch (ex) {
      alert(ex.response?.data || "Произошла ошибка");
    }
  };

  const initPage = async () => {
    try {
      await fetchBalances();

      availableResources.value = (await axios.get('/api/resource/active')).data;
      availableMeasureUnits.value = (await axios.get('/api/measure-unit/active')).data;

      loading.value = false;
    } catch (ex) {
      alert(ex.response?.data || "Произошла ошибка");
    }
  }

  onMounted(async () => {
    await initPage();
  })
</script>
