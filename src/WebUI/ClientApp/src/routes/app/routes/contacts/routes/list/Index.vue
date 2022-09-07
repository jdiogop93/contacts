<template>
  <crud
    :prefix="prefix"
    :path="path"
    :paths="paths"
    :page-title="pageTitle"
    :fields-info="fieldsInfo"
    :details-title="$t('detailsTitle')"
    :export-button="exportButton"
    :refresh-button="refreshButton"
    :select-many-mode="selectManyMode"
    :update-many-mode="updateManyMode"
    :remove-many-mode="removeManyMode"
    :field-filters="fieldFilters"
    deleteMode="hard"
  >
  </crud>
</template>

<script>
import Crud from '@/utils/crud/components/Crud.vue'

export default {
  data () {
    return {
      prefix: 'crud/contacts',
      path: '',
      paths: {
        i: 'contacts/list',
        st: 'contacts/list',
        u: 'contacts/list',
      },
      pageTitle: 'contacts.list',
      exportButton: false,
      refreshButton: false,
      selectManyMode: false,
      updateManyMode: false,
      removeManyMode: false,
      fieldFilters: false,
    }
  },
  computed: {
    fieldsInfo () {
      return [
        {
          text: this.$t('fields.photo'),
          name: 'photo',
          create: false,
          details: false,
          edit: false,
          multiedit: false,
          disableSort: true,
        },
        {
          type: 'input',
          column: 'name',
          text: this.$t('fields.name'),
          name: 'name',
          multiedit: false,
        },
        {
          type: 'input',
          column: 'phone',
          text: this.$t('fields.phone'),
          name: 'phone',
          create: false,
          details: false,
          edit: false,
          multiedit: false,
        },
      ]
    },
  },
  components: {
    Crud,
  },
  i18n: {
    messages: {
      en: {
        detailsTitle: 'Contact',
        fields: {
          id: 'Id',
          photo: 'Photo',
          name: 'Name',
          phone: 'Phone',
        },
      },
    },
  },
}

</script>
