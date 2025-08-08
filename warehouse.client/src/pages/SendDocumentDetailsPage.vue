<template>
  <div class="card shadow-sm">
    <div class="card-body align-items-center">
      <form class="w-100 py-3" @submit.prevent="updateDocument()">
        <span class="fs-3 text-center">Изменение документа отгрузки</span>
        <div class="col">
          <input class="form-control w-100 mt-3" type="text" v-model="document.number" placeholder="Введите номер" />
          <input class="form-control w-100 mt-3" type="date" v-model="document.date" placeholder="Введите дату" />
          <label class="d-inline-block mt-3" for="client">Клиент:</label>
          <select id="client" v-model="document.client" class="form-select mt-1">
            <option selected :value="document.client">{{ document.client.name }}</option>
            <option v-for="client in availableClients" :key="client.guid" :value="client">
              {{ client.name }}
            </option>
          </select>
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
                <td><input type="number" class="form-control" v-model="resource.quantity" /></td>
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
    <div class="card-header">
      Текущий баланс ресурсов
    </div>
    <table class="table table-bordered">
      <thead>
        <tr>
          <th>Ресурс</th>
          <th>Ед. измерения</th>
          <th>Количество</th>
          <th>Доступно</th>
          <th></th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="balance in availableBalances">
          <td>{{ balance.resource.name }}</td>
          <td>{{ balance.measureUnit.name }}</td>
          <td><input type="number" class="form-control" v-model="balance.selectedQuantity" /></td>
          <td>{{ balance.quantity }}</td>
          <td><button @click="addResource(balance.resource, balance.measureUnit, balance.selectedQuantity)" class="btn btn-success">+</button></td>
        </tr>
      </tbody>
    </table>
  </div>
</template>

<script setup>
  import { ref, onMounted } from 'vue';
  import axios from 'axios';
  import router from '@/router';
  import { useRoute } from 'vue-router';

  const route = useRoute();

  const availableClients = ref([]);
  const availableBalances = ref([]);

  const documentId = route.params.id;
  const document = ref({
    number: '',
    date: '',
    client: {},
    resources: []
  });

  const addResource = (resource, measureUnit, quantity) => {
    if (!quantity || quantity <= 0)
      return;

    document.value.resources.push({
      resource: resource,
      measureUnit: measureUnit,
      quantity: quantity
    });
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
      document.value = (await axios.get(`/api/send-document/${documentId}`)).data;
      document.value.date = document.value.date.split('T')[0];
    } catch (ex) {
      alert(ex.response?.data || "Произошла ошибка");
    }
  }

  const updateDocument = async () => {
    try {
      await axios.put('/api/send-document/', document.value);

      router.push('/send-documents');
    } catch (ex) {
      alert(ex.response?.data || "Произошла ошибка");
    }
  }

  onMounted(async () => {
    try {
      availableClients.value = (await axios.get('/api/client/active')).data;
      availableBalances.value = (await axios.get('/api/balance/all')).data;

      await fetchDocument();
    } catch (ex) {
      alert(ex.response?.data || "Произошла ошибка");
    }
  });
</script>
