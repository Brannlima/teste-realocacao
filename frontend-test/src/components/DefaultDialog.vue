<template>
  <v-dialog v-model="dialog" persistent width="1024">
    <v-card>
      <v-card-title>
        <span class="text-h5">{{ action }} {{ type }}</span>
      </v-card-title>
      <v-card-text>
        <v-container>
          <v-row>
            <v-col cols="12" sm="2" md="4" v-if="isFornecedor()">
              <v-text-field
                v-model="nome.value.value"
                :error-messages="nome.errorMessage.value"
                label="Nome"
                variant="outlined"
              ></v-text-field>
            </v-col>
            <v-col cols="12" sm="22" md="8" v-else>
              <v-text-field
                v-model="nome.value.value"
                :error-messages="nome.errorMessage.value"
                label="Nome Fantasia"
                variant="outlined"
              ></v-text-field>
            </v-col>

            <v-col cols="12" sm="2" md="4" v-if="isFornecedor()">
              <v-text-field
                v-model="email.value.value"
                :error-messages="email.errorMessage.value"
                label="Email"
                variant="outlined"
              ></v-text-field>
            </v-col>
            <v-col cols="12" sm="2" md="4">
              <v-text-field
                v-model="cep.value.value"
                :error-messages="cep.errorMessage.value"
                label="CEP"
                variant="outlined"
              ></v-text-field>
            </v-col>
            <v-col cols="12" sm="2" md="8" v-if="isFornecedor() && isCompany">
              <v-text-field
                v-model="cnpj.value.value"
                :error-messages="cnpj.errorMessage.value"
                label="CNPJ"
                variant="outlined"
                :disabled="dialogEdit ? true : false"
              ></v-text-field>
            </v-col>
            <v-col cols="12" sm="2" md="6" v-if="!isFornecedor()">
              <v-text-field
                v-model="cnpj.value.value"
                :error-messages="cnpj.errorMessage.value"
                label="CNPJ"
                variant="outlined"
                :disabled="dialogEdit ? true : false"
              ></v-text-field>
            </v-col>
            <v-col cols="12" sm="2" md="6" v-if="!isFornecedor()">
              <v-autocomplete
                v-model="selectFornecedores.value.value"
                :items="fornecedores"
                chips
                closable-chips
                variant="outlined"
                item-title="nome"
                item-value="fornecedorId"
                label="Fornecedores"
                multiple
                clearable
              >
                <template v-slot:chip="{ props, item }">
                  <v-chip
                    v-bind="props"
                    :text="`${item.raw.nome} | CNPJ/CPF: ${
                      item.raw.cnpj || item.raw.cpf
                    }`"
                  ></v-chip>
                </template>

                <template v-slot:item="{ props, item }">
                  <v-list-item
                    v-bind="props"
                    :title="item?.raw?.nome"
                    :subtitle="
                      item?.raw?.cnpj
                        ? `CNPJ: ${item?.raw?.cnpj}`
                        : `CPF: ${item?.raw?.cpf}`
                    "
                  ></v-list-item>
                </template>
              </v-autocomplete>
            </v-col>
            <v-col cols="12" sm="2" md="8" v-if="isFornecedor() && !isCompany">
              <v-text-field
                v-model="cpf.value.value"
                :error-messages="cpf.errorMessage.value"
                label="CPF"
                variant="outlined"
                :disabled="dialogEdit ? true : false"
              ></v-text-field>
            </v-col>
            <v-col cols="12" sm="6" md="2" v-if="isFornecedor()">
              <v-checkbox v-model="isCompany" label="Empresarial?"></v-checkbox>
            </v-col>
            <v-col cols="12" sm="2" md="4" v-if="isFornecedor() && !isCompany">
              <v-text-field
                v-model="rg.value.value"
                :error-messages="rg.errorMessage.value"
                label="RG"
                variant="outlined"
              ></v-text-field>
            </v-col>
            <v-col cols="12" sm="2" md="4" v-if="isFornecedor() && !isCompany">
              <v-text-field
                v-model="dataNasc.value.value"
                :error-messages="dataNasc.errorMessage.value"
                label="Data de Nascimento"
                variant="outlined"
              ></v-text-field>
            </v-col>
          </v-row>
        </v-container>
        <small>{{ test }}</small>
      </v-card-text>
      <v-card-actions>
        <v-spacer></v-spacer>
        <v-btn color="secondary" variant="elevated" @click="closeDialog">
          Fechar
        </v-btn>
        <v-btn color="primary" variant="elevated" @click="submit">
          Salvar
        </v-btn>
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>

<script>
import { ACTIONS, TYPES } from "@/enum";
import { ref, watch, onBeforeMount } from "vue";
import { useField, useForm } from "vee-validate";
import { formatDateToISO } from "@/helper";
import { getFornecedores, addItem, editItem } from "@/api";

export default {
  props: {
    type: String,
    action: String,
    editItem: Object,
    dialogEdit: Boolean,
  },
  setup(props, context) {
    const isCompany = ref(true);
    const types = ref(TYPES);
    const fornecedores = ref([]);

    onBeforeMount(async () => {
      await setFornecedores();
    });

    const setFornecedores = async () => {
      fornecedores.value = await getFornecedores();
    };

    const nome = useField("nome", (value) => {
      if (!!value) {
        return true;
      } else {
        return "Campo nome é obrigatório.";
      }
    });
    const email = useField("email", (value) => {
      if (/^[a-z.-]+@[a-z.-]+\.[a-z]+$/i.test(value)) {
        return true;
      } else {
        return "E-mail inválido.";
      }
    });
    const cep = useField("cep", (value) => {
      if (/^\d{5}-?\d{3}$/.test(value)) {
        return true;
      } else {
        return "CEP inválido.";
      }
    });
    const cnpj = useField("cnpj", (value) => {
      if (/^\d{2}\.\d{3}\.\d{3}\/\d{4}\-\d{2}$/.test(value)) {
        return true;
      } else {
        return "Insira um CNPJ válido";
      }
    });
    const cpf = useField("cpf", (value) => {
      if (/^\d{3}\.\d{3}\.\d{3}\-\d{2}$/.test(value)) {
        return true;
      } else {
        return "Insira um CPF válido.";
      }
    });
    const rg = useField("rg", (value) => {
      if (!!value) {
        return true;
      } else {
        return "RG não pode estar vazio.";
      }
    });
    const dataNasc = useField("dataNasc", (value) => {
      if (/(^0[1-9]|[12][0-9]|3[01])\/(0[1-9]|1[0-2])\/(\d{4}$)/.test(value)) {
        return true;
      } else {
        return "Insira uma data válida.";
      }
    });
    const selectFornecedores = useField("selectFornecedores");

    const resetForm = () => {
      nome.resetField();
      email.resetField();
      cep.resetField();
      cpf.resetField();
      dataNasc.resetField();
      rg.resetField();
      cnpj.resetField();
      selectFornecedores.resetField();
    };

    const editForm = () => {
      props.editItem.cnpj
        ? (isCompany.value = true)
        : (isCompany.value = false);
      nome.setValue(props.editItem.nome || props.editItem.nomeFantasia || null);
      email.setValue(props.editItem.email || null);
      cep.setValue(props.editItem.cep || null);
      cpf.setValue(props.editItem.cpf || null);
      dataNasc.setValue(props.editItem.dataNascimento || null);
      rg.setValue(props.editItem.rg || null);
      cnpj.setValue(props.editItem.cnpj || null);
      selectFornecedores.setValue(props.editItem.fornecedores || null);
    };

    const cleanErros = () => {
      nome.errors.value = [];
      email.errors.value = [];
      cep.errors.value = [];
      cpf.errors.value = [];
      dataNasc.errors.value = [];
      rg.errors.value = [];
      cnpj.errors.value = [];
    };

    const { handleSubmit } = useForm();

    const isFornecedor = () => {
      return props.type === types.value.fornecedor;
    };

    const closeDialog = () => {
      resetForm();
      context.emit("closeDialog");
    };

    const appendItem = async (body, urlType) => {
      const newItem = await addItem(body, urlType);
      context.emit("addItemOnTable", newItem, !isFornecedor());
    };

    const changeItem = async (body, urlType) => {
      let id = props.editItem.fornecedorId || props.editItem.empresaId;
      if (!isFornecedor()) {
        body.fornecedores.forEach((element) => {
          element.fornecedorId = element.fornecedorId.fornecedorId;
        });
      }
      const editedItem = await editItem(body, id, urlType);
      console.log(body);
    };

    const submit = handleSubmit(async () => {
      cleanErros();

      let apiUrlType;
      let body = {};

      if (isFornecedor() && isCompany.value) {
        await Promise.all([
          nome.validate(),
          email.validate(),
          cep.validate(),
          cnpj.validate(),
        ]);
        apiUrlType = types.value.fornecedores;
        body = {
          Nome: nome.value.value,
          Email: email.value.value,
          CEP: cep.value.value,
          CNPJ: cnpj.value.value,
        };
      } else if (isFornecedor() && !isCompany.value) {
        await Promise.all([
          nome.validate(),
          email.validate(),
          cep.validate(),
          cpf.validate(),
          rg.validate(),
          dataNasc.validate(),
        ]);
        let dateFormat =
          dataNasc.value.value && formatDateToISO(dataNasc.value.value);

        apiUrlType = types.value.fornecedores;
        body = {
          Nome: nome.value.value,
          Email: email.value.value,
          CEP: cep.value.value,
          RG: rg.value.value,
          CPF: cpf.value.value,
          DataNascimento: dateFormat,
        };
      } else {
        await Promise.all([nome.validate(), cep.validate(), cnpj.validate()]);
        let arFornecedores = selectFornecedores.value.value?.map((element) => {
          return { fornecedorId: element };
        });

        apiUrlType = types.value.empresas;
        body = {
          nomeFantasia: nome.value.value,
          cep: cep.value.value,
          cnpj: cnpj.value.value,
          fornecedores: arFornecedores,
        };
      }

      if (
        !nome.errors.value.length > 0 &&
        !email.errors.value.length > 0 &&
        !cep.errors.value.length > 0 &&
        !cpf.errors.value.length > 0 &&
        !dataNasc.errors.value.length > 0 &&
        !rg.errors.value.length > 0 &&
        !cnpj.errors.value.length > 0
      ) {
        props.dialogEdit
          ? changeItem(body, apiUrlType)
          : appendItem(body, apiUrlType);
        closeDialog();
      }
    });

    watch(isCompany, () => {
      props.dialogEdit ? editForm() : resetForm();
    });

    watch(
      () => props.dialogEdit,
      () => {
        props.dialogEdit ? editForm() : resetForm();
      }
    );

    return {
      nome,
      email,
      cep,
      cnpj,
      rg,
      dataNasc,
      cpf,
      selectFornecedores,
      types,
      fornecedores,
      isCompany,
      submit,
      isFornecedor,
      closeDialog,
      resetForm,
      setFornecedores,
      editForm,
    };
  },
};
</script>
