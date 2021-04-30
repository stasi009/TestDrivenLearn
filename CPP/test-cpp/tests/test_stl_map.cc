#include <map>
#include "gtest/gtest.h"

using namespace std;

TEST(MapTest, Constructor)
{
    map<char, int> map1 = {
        {'a', 1},
        {'b', 2},
        {'c', 3},
    };

    ASSERT_EQ(map1.size(), 3);
    ASSERT_EQ(map1['a'], 1);
}

TEST(MapTest, Insert)
{
    map<string, int> map1;

    auto [iter1, success1] = map1.insert({"x", 9});
    ASSERT_TRUE(success1);

    auto [iter2, success2] = map1.insert({"x", 99});
    ASSERT_TRUE(!success2);
}