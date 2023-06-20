# CodingK_EventSystem

事件中心、小顶堆定时器，可以取消事件，支持0-5个参数。
事件中心实现效果类似于GameFramework，但不依赖于unityEngine，不Tick；
小顶堆定时器只适合做不需要Tick的工作，比如buff并不适合用，但是活动开启/关闭这一类的任务很适合。
