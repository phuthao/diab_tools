const ID_BRANCH_ID_KEY = 'BRANCH_ID';
const ID_BRANCH_NAME_KEY = 'BRANCH_NAME';

export const getBranch = () => ({
  id: window.localStorage.getItem(ID_BRANCH_ID_KEY),
  name: window.localStorage.getItem(ID_BRANCH_NAME_KEY),
});

export const saveBranch = (Branch) => {
  window.localStorage.setItem(ID_BRANCH_ID_KEY, Branch.id);
  window.localStorage.setItem(ID_BRANCH_NAME_KEY, Branch.name);
};

export const destroyBranch = () => {
  window.localStorage.removeItem(ID_BRANCH_ID_KEY);
  window.localStorage.removeItem(ID_BRANCH_NAME_KEY);
};

export default { getBranch, saveBranch, destroyBranch };
