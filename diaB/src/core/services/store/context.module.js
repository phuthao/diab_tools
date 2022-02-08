import BranchService from '@/core/services/branch.service';

// action types
export const PUSH_ROUTE_TAB_INDEX = 'pushRouteTabIndex';

// mutation types
export const SET_LOADING = 'setLoading';
export const SET_BRANCH = 'setBranch';
export const SET_UPLOAD_PURCHASED_DELIVERY_BUTTON =
  'setUploadPurchasedDeliveryButton';

export const ADD_ROUTE_TAB_INDEX = 'addRouteTabIndex';
export const SET_ROUTE_TAB_INDEX = 'setRouteTabIndex';
const state = {
  loading: false,
  branch: BranchService.getBranch(),
  routeTabIndex: [],
  uploadPurchasedDeliveryButton: false,
};

const getters = {
  loading(state) {
    return state.loading;
  },
  branch(state) {
    return state.branch;
  },
  routeTabIndex(state) {
    return state.routeTabIndex;
  },
  uploadPurchasedDeliveryButton(state) {
    return state.uploadPurchasedDeliveryButton;
  },
};

const actions = {
  [PUSH_ROUTE_TAB_INDEX](context) {
    if (context.state.routeTabIndex.length) {
      context.commit(ADD_ROUTE_TAB_INDEX);
    } else {
      context.commit(SET_ROUTE_TAB_INDEX);
    }
  },
};

const mutations = {
  [ADD_ROUTE_TAB_INDEX](state) {
    let length = state.routeTabIndex.length;
    state.routeTabIndex.push(length);
  },
  [SET_ROUTE_TAB_INDEX](state) {
    state.routeTabIndex = [0];
  },
  [SET_LOADING](state, loading) {
    state.loading = loading;
  },
  [SET_BRANCH](state, branch) {
    state.branch = branch;
    BranchService.saveBranch(branch);
  },
  [SET_UPLOAD_PURCHASED_DELIVERY_BUTTON](state, isShow) {
    state.uploadPurchasedDeliveryButton = isShow;
  },
};

export default {
  namespaced: true,
  state,
  actions,
  mutations,
  getters,
};
