import List from './routes/list/Index.vue'
import Groups from './routes/groups/Index.vue'

let contactsRoutes = [
  {
    path: 'list',
    name: 'contacts-list',
    component: List,
  },
  {
    path: 'groups',
    name: 'contacts-groups',
    component: Groups,
  },
]

export default contactsRoutes
