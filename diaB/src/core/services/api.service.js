import axios from 'axios';
import moment from 'moment-timezone';

import Mgr from '@/core/services/security.service';

const STATUS_CODE = {
  BAD_REQUEST: 400,
  UNAUTHORIZED: 401,
  NOT_FOUND: 404,
  FORBIDDEN: 403,
  SERVER_ERROR: 500,
};

var user = new Mgr();

// Set config defaults when creating the instance
const instance = axios.create({
  baseURL: process.env.VUE_APP_API,
});

// Alter defaults after instance has been created
instance.defaults.headers.common['Access-Control-Allow-Origin'] = '*';
instance.defaults.headers.common['Access-Control-Allow-Credentials'] = 'true';
instance.defaults.headers.common['Content-Type'] =
  'application/json; charset=utf-8';
instance.defaults.headers.common['X-Timezone-Offset'] = moment.tz.guess();

// Add a request interceptor
instance.interceptors.request.use(
  // Do something before request is sent
  (config) => config,
  // Do something with request error
  (error) => Promise.reject(error),
);

// Add a response interceptor
instance.interceptors.response.use(
  (response) => {
    // Any status code that lie within the range of 2xx cause this function to trigger
    // Do something with response data
    if (response.status === 200 || response.status === 201) {
      return Promise.resolve(response);
    } else {
      return Promise.reject(response);
    }
  },
  (error) => {
    // Any status codes that falls outside the range of 2xx cause this function to trigger
    // Do something with response error
    if (error.response.status) {
      switch (error.response.status) {
        // Do something with status code
        case STATUS_CODE.BAD_REQUEST:
          break;
        case STATUS_CODE.UNAUTHORIZED:
          // handle save current path and login with redirect url
          break;
        case STATUS_CODE.FORBIDDEN:
          break;
        case STATUS_CODE.NOT_FOUND:
          break;
        case STATUS_CODE.SERVER_ERROR:
          break;
      }
      return Promise.reject(error.response);
    }
  },
);

/**
 * Service to call HTTP request via Axios
 */
export default class ApiService {
  /**
   * Define header HTTP request
   * @param url
   * @param config
   * @returns {*}
   */
  async defineHeaderAxios() {
    await user.getAcessToken().then((acessToken) => {
      instance.defaults.headers.common['Authorization'] =
        'Bearer ' + acessToken;
    });
  }

  /**
   * Send the GET HTTP request
   * @param url
   * @param config
   * @returns {*}
   */
  async get(url, config = {}) {
    await this.defineHeaderAxios();
    try {
      let res = await instance.get(url, { ...config });
      if (res.status === 200 || res.status === 201) {
        return res.data;
      }
    } catch (error) {
      if (error && error.data && error.status !== 401) {
        if (error.data.error) {
          throw error.data.error;
        }
        throw error.data;
      } else {
        if (error.status == 401) {
          await user.signOut();
          throw 'Phiên đăng nhập hết hạn';
        }
        throw 'Server không response';
      }
    }
  }

  /**
   * Set the POST HTTP request
   * @param url
   * @param data
   * @param config
   * @returns {*}
   */
  async post(url, data, config = {}) {
    await this.defineHeaderAxios();
    try {
      let res = await instance.post(url, data, { ...config });
      if (res.status === 200 || res.status === 201) {
        return res.data;
      }
    } catch (error) {
      if (error) {
        if (error.data) {
          if (error.data.error) {
            throw error.data.error;
          }
          throw error.data;
        } else if (error.status === 401) {
          await user.signOut();
          throw 'Phiên đăng nhập hết hạn';
        }
      }
      throw { code: 'unknow', message: 'Server không response' };
    }
  }

  /**
   * Send the PUT HTTP request
   * @param url
   * @param data
   * @param config
   * @returns {*}
   */
  async put(url, data, config = {}) {
    await this.defineHeaderAxios();
    try {
      let res = await instance.put(url, data, { ...config });
      if (res.status === 200 || res.status === 201) {
        return res.data;
      }
    } catch (error) {
      if (error) {
        if (error.data) {
          if (error.data.error) {
            throw error.data.error;
          }
          throw error.data;
        } else if (error.status === 401) {
          await user.signOut();
          throw 'Phiên đăng nhập hết hạn';
        }
      }
      throw { code: 'unknow', message: 'Server không response' };
    }
  }

  /**
   * Send the PATCH HTTP request
   * @param url
   * @param data
   * @param config
   * @returns {*}
   */
  async patch(url, data, config = {}) {
    await this.defineHeaderAxios();
    try {
      let res = await instance.patch(url, data, { ...config });
      if (res.status === 200 || res.status === 201) {
        return res.data;
      }
    } catch (error) {
      if (error.response && error.response.data) {
        if (error.response.data.error) {
          throw error.response.data.error;
        }
        throw error.response.data;
      }
      throw error.response;
    }
  }

  /**
   * Send the DELETE HTTP request
   * @param url
   * @param config
   * @returns {*}
   */
  async delete(url, config = {}) {
    await this.defineHeaderAxios();
    try {
      let res = await instance.delete(url, { ...config });
      if (res.status === 200 || res.status === 201) {
        return res.data;
      }
    } catch (error) {
      if (error.response && error.response.data) {
        if (error.response.data.error) {
          throw error.response.data.error;
        }
        throw error.response.data;
      }
      throw error.response;
    }
  }
}
