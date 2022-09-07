import state from './state'
import getters from './getters'
import mutations from './mutations'
import actions from './actions'

let contacts = {
  namespaced: true,
  state,
  getters,
  mutations,
  actions,
}

export default contacts
