const ID_TOKEN_KEY = 'RE0SHgZ43ckIuQSxWj46KB5KZNlSHN8H';
const CURRENT_PATH_KEY = 'NYVDKj2bFITKXwkbJAeMQy00XFYKMjol';

export const urlSearchParams = (url, name) => {
  const results = new RegExp(`[?&]${name}=([^&#]*)`).exec(url);

  if (results == null) {
    return null;
  }

  return decodeURI(results[1]) || 0;
};

export const getTokenFromRedirectURL = () => {
  const url = window.location.hash.split('#')[1];

  return urlSearchParams(url, 'access_token');
};

export const loginWithRedirectURL = () => {
  const params = {
    redirect_uri: `${window.location.origin}/auth/callback`,
    client_id: process.env.VUE_APP_CLIENT_ID,
    response_type: process.env.VUE_APP_RESPONSE_TYPE,
    scope: process.env.VUE_APP_SCOPE,
    state: process.env.VUE_APP_STATE,
    nonce: process.env.VUE_APP_NONE,
  };
  // convert objec to a query string
  const qs = Object.keys(params)
    .map((key) => `${key}=${params[key]}`)
    .join('&');

  window.location.replace(`https://?${qs}`);
};

export const getCurrentPath = () =>
  window.localStorage.getItem(CURRENT_PATH_KEY);

export const saveCurrentPath = () => {
  const { pathname, search } = window.location;
  window.localStorage.setItem(CURRENT_PATH_KEY, `${pathname}${search}`);
};

export const getToken = () => window.localStorage.getItem(ID_TOKEN_KEY);

export const saveToken = (token) => {
  window.localStorage.setItem(ID_TOKEN_KEY, token);
};

export const destroyToken = () => {
  window.localStorage.removeItem(ID_TOKEN_KEY);
};

export default {
  urlSearchParams,
  getTokenFromRedirectURL,
  getToken,
  saveToken,
  destroyToken,
  getCurrentPath,
  saveCurrentPath,
  loginWithRedirectURL,
};
