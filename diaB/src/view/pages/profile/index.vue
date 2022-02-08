<template>
  <Wrapper>
    <div class="profile">
      <div class="profile__header">
        <div class="profile__header-avatar">
          <img :src="picture" alt="" />
        </div>
      </div>
      <div class="profile__body d-flex">
        <div class="col-label px-6" style="border-right: 1px solid #ccc">
          <ul>
            <li v-for="(item, index) in labels" :key="index">
              {{ item }}
            </li>
          </ul>
        </div>
        <div class="col-value px-6">
          <ul>
            <li v-for="(item, index) in values" :key="index">
              {{ item ? item : 'Không' }}
            </li>
          </ul>
        </div>
      </div>
    </div>
  </Wrapper>
</template>

<script>
import { mapGetters } from 'vuex';
export default {
  name: 'Profile',
  data() {
    return {
      labels: [
        'Tên tài khoản',
        'Tên đầy đủ',
        'Tuổi',
        'Số điện thoại',
        'Giới tính',
        'Ngày tạo',
        'Đang hoạt động',
        'Quốc gia',
        'Thành phố/Tỉnh',
        'Quận',
        'Khu vực',
      ],
    };
  },
  computed: {
    ...mapGetters({ profile: 'currentUser' }),

    values() {
      const clone = { ...this.profile };
      const exclude = ['id', 'patientId'];
      let temp = [];
      for (let item in clone) {
        if (!exclude.includes(item)) {
          if (typeof clone[item] == Boolean) {
            temp.push(clone[item].toString());
          } else if (item == 'createDatetime') {
            temp.push(this.$moment.unix(clone[item]).format('MM/DD/YYYY'));
          } else {
            temp.push(clone[item]);
          }
        }
      }
      return temp;
    },

    picture() {
      return process.env.BASE_URL + this.profile && this.profile.image
        ? this.currentUser.image
        : 'media/users/300_21.jpg';
    },
  },

  methods: {},
  created() {},
};
</script>

<style lang="scss" scoped>
.profile {
  li {
    line-height: 3rem;
    list-style-type: none;
  }
  &__header {
    margin-top: -2.25rem !important;
    margin-left: -2.25rem !important;
    margin-right: -2.25rem !important;
    background: #d6d6d6;
    min-height: 150px;
    &-avatar {
      width: 200px;
      height: 200px;
      position: absolute;
      overflow: hidden;
      top: 40px;
      left: 5%;
      img {
        border-radius: 50%;
        height: auto;
        object-fit: cover;
        width: 100%;
        padding: 1rem;
        background: #fff;
      }
    }
  }
  &__body {
    margin-top: 100px;
  }
}
</style>
