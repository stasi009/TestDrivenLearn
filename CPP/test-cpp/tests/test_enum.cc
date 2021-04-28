
#include <map>
#include "gtest/gtest.h"

enum Fruit {Apple,Orange,Banana};

enum class MobilePhone {Apple, Huawei, Xiaomi, Oppo, Vivo};

TEST(EnumTest, WeakTypeEnum)
{
    Fruit a = Apple;
    // 这个测试说明两点：
    // 1. 对于weak typed enum，还是可以视为其背后的真实类型，int的
    // 2. 缺省是从0开始编号
    ASSERT_EQ(a,0);
    ASSERT_EQ(Banana,2);
}

TEST(EnumTest, StrongTypeEnum)
{
    std::map<MobilePhone,std::string> mobile_phone_brands = {
        {MobilePhone::Apple,"Apple"},
        {MobilePhone::Huawei,"Huawei"},
        {MobilePhone::Xiaomi,"Xiaomi"},
        {MobilePhone::Oppo,"Oppo"},
    };

    ASSERT_EQ(mobile_phone_brands[MobilePhone::Oppo],"Oppo");
}