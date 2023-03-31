<template>
  <v-card color="primary">
    <v-card-title class="text-center justify-center py-6">
      <h1 class="font-weight-bold text-navbar text-h3">MyTest</h1>
    </v-card-title>
    <v-tabs
      v-model="tab"
      @update:model-value="tabChanged"
      bg-color="transparent"
      color="tertiary"
      grow
    >
      <v-tab v-for="item in items" :key="item" :value="item">
        {{ item }}
      </v-tab>
    </v-tabs>
  </v-card>
</template>

<script>
export default {
  data() {
    return {
      tab: null,
      items: ["Empresa", "Fornecedor"],
    };
  },
  methods: {
    tabChanged(item) {
      this.$emit("tabSelected", item);
    },
  },
};
</script>

<style>
.text-navbar {
  color: "#FAFAFA" !important;
}
</style>
