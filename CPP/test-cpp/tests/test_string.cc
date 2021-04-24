
#include "gtest/gtest.h"
#include <map>

using namespace ::std;

TEST(StringTest, Construct)
{
    string s1("abc");
    ASSERT_EQ(s1.size(), 3);

    string s2 = "xy";
    ASSERT_EQ(s2.size(), 2);
}

TEST(StringTest, ImplictConversion)
{
    // 从c-str literal到string有implicit conversion
    // 所以下面的代码中，尽管声明key的类型是std::string
    // 但是可以直接有c-style string来插入和检索
    map<string, int> map1;
    map1.insert({"abc", 89});

    int v = map1["abc"];
    ASSERT_EQ(v, 89);
}