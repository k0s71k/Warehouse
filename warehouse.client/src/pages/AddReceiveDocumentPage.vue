<template>
  <div class="card shadow-sm">
    <div class="card-body align-items-center">
      <form class="w-100 py-3" @submit.prevent="addDocument()">
        <span class="fs-3 text-center">Добавление документа приема</span>
        <div class="col">
          <input class="form-control w-100 mt-3" type="text" v-model="document.number" placeholder="Введите номер" />
          <input class="form-control w-100 mt-3" type="date" v-model="document.date" placeholder="Введите дату" />
          <table class="table">
            <thead>
              <tr>
                <th></th>
                <th>Ресурс</th>
                <th>Ед. измерения</th>
                <th>Количество</th>
              </tr>
            </thead>
            <tbody>
              <tr v-for="(resource, index) in document.resources">
                <td><button @click="deleteResource(index)" class="btn btn-danger">Х</button></td>
                <td>{{ resource.resource.name }}</td>
                <td>{{ resource.measureUnit.name }}</td>
                <td>{{ resource.quantity }}</td>
              </tr>
            </tbody>
          </table>
          <button class="btn btn-success w-100 mt-3" type="submit">
            Сохранить
          </button>
        </div>
      </form>
    </div>
  </div>

  <div class="card shadow-sm mt-4">
    <div class="card-body row">
      <div class="col-md-4">
        <label class="d-inline-block" for="resource">Ресурс:</label>
        <select id="resource" v-model="selectedResource.resource" class="form-select mt-1">
          <option selected v-for="resource in availableResources" :key="resource.guid" :value="resource">
            {{ resource.name }}
          </option>
        </select>
      </div>
      <div class="col-md-4">
        <label class="d-inline-block" for="measureUnit">Единица измерения:</label>
        <select id="measureUnit" v-model="selectedResource.measureUnit" class="form-select mt-1">
          <option selected v-for="measureUnit in availableMeasureUnits" :key="measureUnit.guid" :value="measureUnit">
            {{ measureUnit.name }}
          </option>
        </select>
      </div>
      <div class="col-md-4">
        <label class="d-inline-block" for="quantity">Количество:</label>
        <input id="quantity" type="number" class="form-control" v-model="selectedResource.quantity" />

        <button @click="addResource()" class="btn btn-success mt-3 w-100">Добавить ресурс</button>
      </div>
    </div>
  </div>
</template>

<script setup>
  import { ref, onMounted } from 'vue';
  import axios from 'axios';
  import router from '@/router';

  const availableResources = ref([]);
  const availableMeasureUnits = ref([]);

  const document = ref({
    number: '',
    date: '',
    resources: []
  });

  const selectedResource = ref({
    resource: null,
    measureUnit: null,
    quantity: 0
  });

  const addResource = () => {
    if (selectedResource.value.quantity <= 0)
      return;

    document.value.resources.push(selectedResource.value);
    selectedResource.value = {
      resource: null,
      measureUnit: null,
      quantity: 0
    }
  }

  const deleteResource = (index) => {
    try {
      document.value.resources.splice(index, 1);
    } catch (ex) {
      alert(ex.response?.data || "Произошла ошибка");
    }
  }

  const addDocument = async () => {
    try {
      await axios.post('/api/receive-document/add', document.value);
      router.push('/receive-documents');
    } catch (ex) {
      alert(ex.response?.data || "Произошла ошибка");
    }
  }

  onMounted(async () => {
    try {
      availableResources.value = (await axios.get('/api/resource/active')).data;
      availableMeasureUnits.value = (await axios.get('/api/measure-unit/active')).data;
    } catch (ex) {
      alert(ex.response?.data || "Произошла ошибка");
    }
  });
</script>
