load("@rules_cc//cc:defs.bzl", "cc_test")

cc_test(
    name = "testcpp",
    # tests这个package下的任意子目录下的cc文件都要参与编译
    srcs = glob(["**/*.cc"]),
    deps = [
        "//src/lib:CPPLib",
        "@com_google_googletest//:gtest_main",
    ],
)


