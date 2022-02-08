import ApiService from '@/core/services/api.service';

// action types
export const VERIFY_AUTH = 'verifyAuth';
export const LOGIN = 'login';
export const LOGOUT = 'logout';
export const REGISTER = 'register';
export const GET_USER = 'getUser';
export const UPDATE_USER = 'updateUser';

// mutation types
export const PURGE_AUTH = 'logOut';
export const SET_AUTH = 'setUser';
export const SET_ERROR = 'setError';

const $api = new ApiService();

const state = {
  errors: null,
  user: {},
  isAuthenticated: true,
};
const getters = {
  currentUser(state) {
    return state.user;
  },
  isAuthenticated(state) {
    return state.isAuthenticated;
  },
};

const actions = {
  [LOGIN](context, credentials) {
    return new Promise((resolve) => {
      $api
        .post('login', credentials)
        .then(({ data }) => {
          context.commit(SET_AUTH, data);
          resolve(data);
        })
        .catch(({ response }) => {
          context.commit(SET_ERROR, response.data.errors);
        });
    });
  },
  [LOGOUT](context) {
    context.commit(PURGE_AUTH);
  },
  [REGISTER](context, credentials) {
    return new Promise((resolve, reject) => {
      $api
        .post('users', { user: credentials })
        .then(({ data }) => {
          context.commit(SET_AUTH, data);
          resolve(data);
        })
        .catch(({ response }) => {
          context.commit(SET_ERROR, response.data.errors);
          reject(response);
        });
    });
  },
  [GET_USER](context) {
    return new Promise((resolve) => {
      $api
        .get('Admin/Account/Current')
        .then(({ data }) => {
          context.commit(SET_AUTH, data);
          resolve(data);
        })
        .catch(({ response }) => {
          context.commit(SET_ERROR, response.data.errors);
        });
    });
  },
  [UPDATE_USER](context, payload) {
    const { email, username, password, image, bio } = payload;
    const user = { email, username, bio, image };
    if (password) {
      user.password = password;
    }

    return $api.put('user', user).then(({ data }) => {
      context.commit(SET_AUTH, data);
      return data;
    });
  },
};

const mutations = {
  [SET_ERROR](state, error) {
    state.errors = error;
  },
  [SET_AUTH](state, user) {
    state.isAuthenticated = true;
    state.user = user;
    state.errors = {};
  },
  [PURGE_AUTH](state) {
    state.isAuthenticated = false;
    state.user = {};
    state.errors = {};
  },
};

export default {
  state,
  actions,
  mutations,
  getters,
};
