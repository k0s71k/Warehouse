<template>
  <header class="shadow-lg">
    <nav class="navbar navbar-expand-lg">
      <div class="container">
        <router-link to="/" class="navbar-brand">
          <img alt="Warehouse Control" class="logo" src="@/assets/logo.svg" width="45" height="45" />
        </router-link>

        <div class="collapse navbar-collapse justify-content-end">
          <ul class="navbar-nav">
            <li class="nav-item dropdown">
              <a class="nav-link dropdown-toggle" id="referenceDropdown" role="button" data-bs-toggle="dropdown" aria-expanded="false">
                Справочник
              </a>
              <ul class="dropdown-menu dropdown-menu-end" aria-labelledby="referenceDropdown">
                <li>
                  <router-link to="/clients" class="dropdown-item">
                    Клиенты
                  </router-link>
                </li>
                <li>
                  <router-link to="/measure-units" class="dropdown-item">
                    Единицы измерения
                  </router-link>
                </li>
                <li>
                  <router-link to="/resources" class="dropdown-item">
                    Ресурсы
                  </router-link>
                </li>
              </ul>
            </li>
          </ul>

          <ul class="navbar-nav">
            <li class="nav-item dropdown">
              <a class="nav-link dropdown-toggle" id="referenceDropdown" role="button" data-bs-toggle="dropdown" aria-expanded="false">
                Склад
              </a>
              <ul class="dropdown-menu dropdown-menu-end" aria-labelledby="referenceDropdown">
                <li>
                  <router-link to="/balances" class="dropdown-item">
                    Баланс
                  </router-link>
                </li>
                <li>
                  <router-link to="/receive-documents" class="dropdown-item">
                    Поступления
                  </router-link>
                </li>
                <li>
                  <router-link to="/send-documents" class="dropdown-item">
                    Отгрузки
                  </router-link>
                </li>
              </ul>
            </li>
          </ul>
        </div>
      </div>
    </nav>
  </header>
</template>
