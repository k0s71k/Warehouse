<template>
  <div v-if="loading" class="text-center py-5">
    <div class="spinner-border text-primary" role="status"></div>
  </div>
  <div v-else>
    <div class="card shadow-sm">
      <div class="card-body col">
        <span class="fs-3">Список клиентов</span>
        <div class="row mt-3">
          <div class="col-md-4">
            <button class="btn btn-success w-100" @click="router.push('/clients/add')">
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
              <th>Адрес</th>
              <th>В архиве</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="client in clients" :key="client.guid">
              <td><button @click="deleteClient(client.guid)" class="btn btn-danger">Х</button></td>
              <td><input class="form-control w-100" v-model="client.name" /></td>
              <td><input class="form-control w-100" v-model="client.address" /></td>
              <td><input type="checkbox" class="w-100 btn form-check" v-model="client.isArchieved" /></td>
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
  const clients = ref([]);
  const showArchieved = ref(false);

  const showArchievedText = computed(() =>
    showArchieved.value ? "Показать активных клиентов" : "Показать архив клиентов");

  const archievedClick = async () => {
    showArchieved.value = !showArchieved.value;
    await fetchClients();
  }

  const applyClick = async () => {
    try {
      await axios.put('/api/client/update', clients.value);
      await fetchClients();
    } catch (ex) {
      alert(ex.response?.data || "Произошла ошибка");
    }
  }

  const fetchClients = async () => {
    try {
      const response = await axios.get(
        showArchieved.value ? '/api/client/archieved' : '/api/client/active');

      clients.value = response.data;
    } catch (ex) {
      alert(ex.response?.data || "Произошла ошибка");
    }
  }

  const deleteClient = async (id) => {
    try {
      await axios.delete(`/api/client/${id}`);
      await fetchClients();
    } catch (ex) {
      alert(ex.response?.data || "Произошла ошибка");
    }
  }

  onMounted(async () => {
    loading.value = true;
    await fetchClients();
    loading.value = false;
  })
</script>
