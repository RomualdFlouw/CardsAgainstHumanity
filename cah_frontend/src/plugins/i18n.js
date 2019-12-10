import Vue from 'vue'
import VueI18n from 'vue-i18n'

import en from "../assets/translations/Translation_English"
import nl from "../assets/translations/Translation_Nederlands"
Vue.use(VueI18n)

let languageFiles = {
    en: en,
    nl: nl,
}

const currentlocale = localStorage.getItem("locale");

export const i18n = new VueI18n({
    locale : currentlocale,
    messages : languageFiles
})