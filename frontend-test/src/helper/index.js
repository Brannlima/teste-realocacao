export const formatDateToISO = (date) => {
  return new Date(date.split("/").reverse()).toISOString();
};

export const formatDataNasc = (data) => {
  data.forEach((element) => {
    let initialDate = new Date(element.dataNascimento);
    let dateFormat =
      initialDate.getDate() +
      "/" +
      (initialDate.getMonth() + 1) +
      "/" +
      initialDate.getFullYear();

    element.dataNascimento = dateFormat;
    if (element.dataNascimento === "1/1/1") {
      element.dataNascimento = null;
    }
  });

  return data;
};

export const formatEmpresaObject = (data) => {
  data.forEach((element) => {
    element.fornecedores = element.fornecedores.$values;
    element.countFornecedores = element.fornecedores.length;
    element.fornecedores = formatDataNasc(element.fornecedores);
  });
  return data;
};
