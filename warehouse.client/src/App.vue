<script setup>
  import HeaderComponent from '@/components/HeaderComponent.vue'
  import FooterComponent from '@/components/FooterComponent.vue'
</script>

<template>
  <div class="app-layout">
    <HeaderComponent />

    <main class="main-content">
      <div class="container py-5">
        <router-view />
      </div>
    </main>

    <FooterComponent />
  </div>
</template>

<style scoped>
</style>
