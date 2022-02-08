// action types
export const GET_PARAMS = 'getParams';
export const SET_PARAMS = 'setParams';

import ApiService from '@/core/services/api.service';
const $api = new ApiService();

export default {
  state: {
    hotline: null,
  },
  getters: {
    getHotline: (state) => state.hotline,
  },
  actions: {
    [SET_PARAMS]({ commit }) {
      commit(SET_PARAMS);
    },
  },
  mutations: {
    [SET_PARAMS](state) {
      return new Promise(() => {
        $api
          .get('CommonConfigureTask/b9cad950-f688-4124-8c9e-688d568b3e8d')
          .then(({ data }) => {
            state.hotline = data?.value;
          })
          .catch(() => {});
      });
    },
  },
};
