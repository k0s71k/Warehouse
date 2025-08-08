<template>
  <div v-if="loading" class="text-center py-5">
    <div class="spinner-border text-primary" role="status"></div>
  </div>
  <div v-else class="card shadow-sm p-3">
    <div class="row">
      <div class="col-md-8">
        <span class="fs-4">Документы приема</span>
      </div>
      <div class="col-md-4">
        <button @click="router.push('receive-documents/add')" type="submit" class="btn btn-success w-100">Добавить</button>
      </div>
    </div>
    
    <form class="my-3" @submit.prevent="fetchDocuments()">
      <div class="row">
        <div class="col-md-6">
          <label class="d-inline-block" for="startDate">Наименьшая дата добавления</label>
          <input class="form-control w-100" id="startDate" type="date" v-model="filter.dateStart" />
        </div>
        <div class="col-md-6">
          <label class="d-inline-block" for="endDate">Наибольшая дата добавления</label>
          <input class="form-control w-100" id="endDate" type="date" v-model="filter.dateEnd" />
        </div>
      </div>
      <div class="row mt-3">
        <div class="col-md-4">
          <label class="d-inline-block" for="number">Поиск по номеру:</label>
          <select id="number" v-model="filter.numbers" class="form-select mt-1" multiple>
            <option v-for="number in availableDocumentNumbers" :key="number" :value="number">
              {{ number }}
            </option>
          </select>
        </div>
        <div class="col-md-4">
          <label class="d-inline-block" for="resource">Поиск по ресурсу:</label>
          <select id="resource" v-model="filter.resourceIds" class="form-select mt-1" multiple>
            <option v-for="resource in availableResources" :key="resource.guid" :value="resource.guid">
              {{ resource.name }}
            </option>
          </select>
        </div>
        <div class="col-md-4">
          <label class="d-inline-block" for="measureUnit">Поиск по единице измерения:</label>
          <select id="measureUnit" v-model="filter.measureUnitIds" class="form-select mt-1" multiple>
            <option v-for="measureUnit in availableMeasureUnits" :key="measureUnit.guid" :value="measureUnit.guid">
              {{ measureUnit.name }}
            </option>
          </select>
        </div>
      </div>
      <div class="mt-3 row justify-content-center">
        <div class="col-md-12">
          <button type="submit" class="btn btn-primary w-100">Применить фильтр</button>
        </div>
      </div>
    </form>
    <div class="row card-body">
      <table class="table table-bordered align-middle">
        <thead>
          <tr>
            <th></th>
            <th>Номер</th>
            <th>Дата</th>
            <th>Ресурс</th>
            <th>Ед. измерения</th>
            <th>Количество</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="document in documents" :key="document.guid">
            <td>
              <button @click="deleteDocument(document.guid)" class="btn btn-danger">x</button>
              <button @click="router.push(`/receive-documents/${document.guid}`)" class="btn btn-warning mx-1">...</button>
            </td>
            <td>{{ document.number }}</td>
            <td>{{ document.date.split('T')[0] }}</td>
            <td><tr v-for="resource in document.resources">{{ resource.resource.name }}</tr></td>
            <td><tr v-for="resource in document.resources">{{ resource.measureUnit.name }}</tr></td>
            <td><tr v-for="resource in document.resources">{{ resource.quantity }}</tr></td>
          </tr>
        </tbody>
      </table>
    </div>
  </div>
</template>

<script setup>
  import { ref, onMounted } from 'vue';
  import axios from 'axios';
  import router from '@/router';

  const loading = ref(true);
  const filter = ref({
    dateStart: '2024-01-01',
    dateEnd: '2026-12-31',
    numbers: [],
    resourceIds: [],
    measureUnitIds: [],
  });
  const documents = ref([]);

  const availableResources = ref([]);
  const availableMeasureUnits = ref([]);
  const availableDocumentNumbers = ref([]);

  const deleteDocument = async (id) => {
    try {
      await axios.delete(`/api/receive-document/${id}`);
      await fetchDocuments();
    } catch (ex) {
      alert(ex.response?.data || "Произошла ошибка");
    }
  }

  const fetchDocuments = async () => {
    try {
      availableDocumentNumbers.value = (await axios.get('/api/receive-document/numbers')).data;
      documents.value = (await axios.post('/api/receive-document/filter', filter.value)).data;
    } catch (ex) {
      alert(ex.response?.data || "Произошла ошибка");
    }
  };

  onMounted(async () => {
    try {
      await fetchDocuments();
      availableResources.value = (await axios.get('/api/resource/active')).data;
      availableMeasureUnits.value = (await axios.get('/api/measure-unit/active')).data;

      loading.value = false;
    } catch (ex) {
      alert(ex.response?.data || "Произошла ошибка");
    }
  })
</script>
