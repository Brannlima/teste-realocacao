<template>
  <v-card>
    <v-card-title>
      <div class="card-header d-flex">
        {{ title }}
        <v-spacer></v-spacer>
        <v-btn
          class="add-button"
          color="primary"
          @click="openDialog(null, actions.add)"
          rounded="lg"
        >
          Adicionar
        </v-btn>
        <default-dialog
          v-model="dialog"
          :type="getType()"
          :action="currentAction"
          @close-dialog="closeDialog"
          @add-item-on-table="addItemOnTable"
          :dialogEdit="dialogEdit"
          :editItem="editItem"
        ></default-dialog>
      </div>
      <v-container>
        <v-text-field
          v-model="search"
          append-icon="mdi-magnify"
          label="Pesquisar"
          single-line
          density="compact"
          variant="solo"
          hide-details
        ></v-text-field>
      </v-container>
      <el-table
        :data="searchDataTable"
        style="width: 100%"
        height="600"
        border
        table-layout="auto"
      >
        <el-table-column
          v-for="column in headers"
          :type="column.key === 'fornecedores' && 'expand'"
          :key="column.key"
          :label="column.title"
          :prop="column.key"
          :width="column.key === 'fornecedores' && 180"
        >
          <template #default="props">
            <v-container
              v-if="
                column.key === 'fornecedores' &&
                props.row.fornecedores.length > 0
              "
            >
              <h4>Fornecedores</h4>
              <el-table :data="props.row.fornecedores" border ref="table">
                <el-table-column
                  v-for="column in header"
                  :key="column.key"
                  :label="column.title"
                  :prop="column.key"
                >
                </el-table-column>
              </el-table>
            </v-container>
          </template>
        </el-table-column>
        <el-table-column label="Operations">
          <template #default="scope">
            <v-row justify="center">
              <v-col cols="auto">
                <v-btn
                  class="add-button"
                  color="primary"
                  @click="openDialog(scope.row, actions.edit)"
                  rounded="lg"
                  size="small"
                >
                  Editar
                </v-btn>
              </v-col>

              <v-col cols="auto">
                <v-btn
                  class="add-button"
                  color="secondary"
                  @click="openDialogDelete(scope)"
                  rounded="lg"
                  size="small"
                >
                  Deletar
                </v-btn>
              </v-col>
            </v-row>
          </template>
        </el-table-column>
      </el-table>
    </v-card-title>
  </v-card>
  <v-dialog v-model="dialogDelete" max-width="500px">
    <v-card>
      <v-card-title class="text-h5"
        >Tem certeza que quer excluir este item?</v-card-title
      >
      <v-card-actions>
        <v-spacer></v-spacer>
        <v-btn color="secondary" variant="outlined" @click="closeDialogDelete"
          >Cancel</v-btn
        >
        <v-btn
          color="primary"
          variant="outlined"
          @click="removeItem(selectedItem)"
          >Confirmar</v-btn
        >
        <v-spacer></v-spacer>
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>

<script>
import { TYPES, ACTIONS } from "@/enum";
import DefaultDialog from "./DefaultDialog.vue";
import { deleteItem } from "@/api";
import { fornecedorHeaders } from "@/headers";
import { ref, computed, watch } from "vue";

export default {
  props: {
    title: String,
    fornecedores: Array,
    currentValue: Array,
    headers: Array,
  },
  setup(props, context) {
    let currentAction = ref(null);
    const actions = ref(ACTIONS);
    const dialog = ref(false);
    const search = ref("");
    const editItem = ref(null);
    const dialogEdit = ref(false);

    const searchDataTable = computed(() =>
      props.currentValue.filter(
        (data) =>
          !search.value ||
          data.nomeFantasia
            ?.toLowerCase()
            .includes(search.value.toLowerCase()) ||
          data.nome?.toLowerCase().includes(search.value.toLowerCase()) ||
          data.cpf?.toLowerCase().includes(search.value.toLowerCase()) ||
          data.cnpj?.toLowerCase().includes(search.value.toLowerCase())
      )
    );

    const openDialog = (item, action) => {
      if (action === actions.value.edit) {
        dialogEdit.value = true;
        editItem.value = item;
      }
      dialog.value = true;
    };

    const closeDialog = () => {
      dialog.value = false;
      dialogEdit.value = false;
    };

    return {
      closeDialog,
      openDialog,
      editItem,
      dialog,
      currentAction,
      searchDataTable,
      search,
      dialogEdit,
    };
  },
  data: () => ({
    loaded: false,
    loading: true,
    dialogDelete: false,
    types: TYPES,
    actions: ACTIONS,
    selectedItem: null,
    fornecedoresValue: null,
    header: fornecedorHeaders,
  }),
  methods: {
    getType() {
      return this.title;
    },
    isEmpresa() {
      return this.title === this.types.empresa;
    },
    async removeItem(item) {
      let itemType;
      this.isEmpresa() ? (itemType = "empresas") : (itemType = "fornecedores");
      this.currentValue.splice(item.index, 1);
      item = deleteItem(item.id, itemType);
      this.closeDialogDelete();
    },
    openDialogDelete(item) {
      this.dialogDelete = true;
      this.isEmpresa()
        ? (this.selectedItem = { id: item.row.empresaId, index: item.$index })
        : (this.selectedItem = {
            id: item.row.fornecedorId,
            index: item.$index,
          });
    },
    closeDialogDelete() {
      this.dialogDelete = false;
    },
    addItemOnTable(item, isEmpresa) {
      console.log(item);
      if (isEmpresa) {
        item.countFornecedores = item.fornecedores.$values.length;
        item.fornecedores = item.fornecedores.$values;
      }
      this.currentValue.push(item);
    },
  },
  components: { DefaultDialog },
};
</script>

<style>
.card-header {
  flex-direction: row;
  justify-content: space-between;
  margin-bottom: 24px;
  margin-top: 16px;
}
</style>
