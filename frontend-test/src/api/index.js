import axios from "axios";
import { formatEmpresaObject, formatDataNasc } from "@/helper";

export const getFornecedores = async () => {
  try {
    let data;

    await axios
      .get(`${import.meta.env.VITE_API_ENDPOINT}/api/fornecedores`)
      .then((response) => {
        data = formatDataNasc(response.data.$values);
      });
    return data;
  } catch (error) {
    console.error(error);
  }
};

export const getEmpresas = async () => {
  try {
    let data;
    await axios
      .get(`${import.meta.env.VITE_API_ENDPOINT}/api/empresas`)
      .then((response) => {
        data = formatEmpresaObject(response.data.$values);
      });
    return data;
  } catch (error) {
    console.error(error);
  }
};

export const fetchData = async () => {
  const fornecedores = await getFornecedores();
  const empresas = await getEmpresas();

  return { fornecedores, empresas };
};

export const deleteItem = async (id, type) => {
  try {
    await axios.delete(
      `${import.meta.env.VITE_API_ENDPOINT}/api/${type}/${id}`
    );
  } catch (error) {
    console.error(error);
  }
};

export const addItem = async (body, type) => {
  try {
    let data;
    await axios
      .post(`${import.meta.env.VITE_API_ENDPOINT}/api/${type}`, body)
      .then((response) => {
        data = response.data;
      });
    return data;
  } catch (error) {
    console.error(error);
  }
};

export const editItem = async (body, id, type) => {
  try {
    let data;
    await axios
      .put(`${import.meta.env.VITE_API_ENDPOINT}/api/${type}/${id}`, body)
      .then((response) => {
        data = response;
      });
    return data;
  } catch (error) {
    console.error(error);
  }
};
