import "@mdi/font/css/materialdesignicons.css";
import "vuetify/styles";

// Composables
import { createVuetify } from "vuetify";
import * as labs from "vuetify/labs/components";

export default createVuetify({
  components: {
    ...labs,
  },
  theme: {
    themes: {
      light: {
        colors: {
          primary: "#32908F",
          secondary: "#A40E4C",
          tertiary: "#0A090C",
        },
      },
    },
  },
});
