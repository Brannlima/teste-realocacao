<template>
  <default-app-bar @tab-selected="tabSelected"></default-app-bar>
  <v-container>
    <default-table
      :title="tableTitle"
      :headers="headers"
      :current-value="items"
      :fornecedores="fornecedores"
    ></default-table>
  </v-container>
</template>

<script>
import DefaultAppBar from "@/components/DefaultAppBar.vue";
import DefaultTable from "@/components/DefaultTable.vue";
import { TYPES } from "@/enum";
import { fetchData } from "@/api";
import { empresaHeaders, fornecedorHeaders } from "@/headers";
export default {
  data() {
    return {
      tableTitle: null,
      empresasValues: [],
      items: [],
      headers: [],
      types: TYPES,
      requestType: null,
      fornecedoresValues: null,
    };
  },
  methods: {
    async tabSelected(tabItem) {
      this.tableTitle = tabItem;
      this.getHeaders();
      const { empresas, fornecedores } = await fetchData();
      if (this.isEmpresa()) {
        this.items = empresas;
      } else if (!this.isEmpresa()) {
        this.items = fornecedores;
      }
    },
    getHeaders() {
      return this.isEmpresa()
        ? (this.headers = empresaHeaders)
        : (this.headers = fornecedorHeaders);
    },
    isEmpresa() {
      return this.tableTitle === this.types.empresa;
    },
  },
  async mounted() {
    this.getHeaders();
    const { empresas, fornecedores } = await fetchData();
    this.fornecedoresValues = fornecedores;
    this.items = empresas;
  },
  components: {
    DefaultTable,
    DefaultAppBar,
  },
};
</script>

<style></style>
