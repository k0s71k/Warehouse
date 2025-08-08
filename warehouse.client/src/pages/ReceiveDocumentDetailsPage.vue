<template>
  <div class="card shadow-sm">
    <div class="card-body align-items-center">
      <form class="w-100 py-3" @submit.prevent="updateDocument()">
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
                <td>
                  <select v-model="resource.resource" class="form-select">
                    <option selected :value="resource.resource">{{ resource.resource.name }}</option>
                    <option v-for="availableResource in availableResources" :key="availableResource.guid" :value="availableResource">
                      {{ availableResource.name }}
                    </option>
                  </select>
                </td>
                <td>
                  <select v-model="resource.measureUnit" class="form-select">
                    <option selected :value="resource.measureUnit">{{ resource.measureUnit.name }}</option>
                    <option v-for="availableMeasureUnit in availableMeasureUnits" :key="availableMeasureUnit.guid" :value="availableMeasureUnit">
                      {{ availableMeasureUnit.name }}
                    </option>
                  </select>
                </td>
                <td>
                  <input class="form-control" type="number" v-model="resource.quantity" />
                </td>
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
  import { onMounted, ref } from 'vue';
  import axios from 'axios';
  import router from '@/router';
  import { useRoute } from 'vue-router';

  const route = useRoute();

  const availableResources = ref([]);
  const availableMeasureUnits = ref([]);

  const selectedResource = ref({
    resource: null,
    measureUnit: null,
    quantity: 0
  });

  const documentId = route.params.id;
  const document = ref({});

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

  const fetchDocument = async () => {
    try {
      document.value = (await axios.get(`/api/receive-document/${documentId}`)).data;
      document.value.date = document.value.date.split('T')[0];
    } catch (ex) {
      alert(ex.response?.data || "Произошла ошибка");
    }
  };

  const updateDocument = async () => {
    try {
      await axios.put('/api/receive-document/', document.value);

      router.push('/receive-documents');
    } catch (ex) {
      alert(ex.response?.data || "Произошла ошибка");
    }
  };

  onMounted(async () => {
    try {
      availableResources.value = (await axios.get('/api/resource/active')).data;
      availableMeasureUnits.value = (await axios.get('/api/measure-unit/active')).data;

      await fetchDocument();
    } catch (ex) {
      alert(ex.response?.data || "Произошла ошибка");
    }
  })
</script>
