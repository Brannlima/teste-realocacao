import { createApp } from "vue";
import { registerPlugins } from "@/plugins";
import ElementPlus from "element-plus";
import "element-plus/dist/index.css";
import App from "./App.vue";

const app = createApp(App);

app.use(ElementPlus);

if (process.env.ENV === "dev") {
  app.config.devtools = true;
}

registerPlugins(app);

app.mount("#app");
