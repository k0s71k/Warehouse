<template>
  <div v-if="loading" class="text-center py-5">
    <div class="spinner-border text-primary" role="status"></div>
  </div>
  <div v-else>
    <div class="card shadow-sm">
      <div class="card-body col">
        <span class="fs-3">Список ресурсов</span>
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
            <tr v-for="resource in resources" :key="resource.guid">
              <td><button @click="deleteResource(resource.guid)" class="btn btn-danger">Х</button></td>
              <td><input class="form-control w-100" v-model="resource.name" /></td>
              <td><input type="checkbox" class="w-100 btn form-check" v-model="resource.isArchieved" /></td>
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
  const resources = ref([]);
  const showArchieved = ref(false);

  const showArchievedText = computed(() =>
    showArchieved.value ? "Показать активные ресурсы" : "Показать архив ресурсов");

  const addUserClick = () => {
    router.push('/resources/add');
  }
  const archievedClick = async () => {
    showArchieved.value = !showArchieved.value;
    await fetchResources();
  }
  const applyClick = async () => {
    try {
      await axios.put('/api/resource/update', resources.value);
      await fetchResources();
    } catch (ex) {
      alert(ex.response?.data || "Произошла ошибка");
    }
  }

  const fetchResources = async () => {
    try {
      const response = await axios.get(
        showArchieved.value ? '/api/resource/archieved' : '/api/resource/active');

      resources.value = response.data;
    } catch (ex) {
      alert(ex.response?.data || "Произошла ошибка");
    }
  }

  const deleteResource = async (id) => {
    try {
      await axios.delete(`/api/resource/${id}`)
      await fetchResources();
    } catch (ex) {
      alert(ex.response?.data || "Произошла ошибка");
    }
  }

  onMounted(async () => {
    loading.value = true;
    await fetchResources();
    loading.value = false;
  })
</script>
