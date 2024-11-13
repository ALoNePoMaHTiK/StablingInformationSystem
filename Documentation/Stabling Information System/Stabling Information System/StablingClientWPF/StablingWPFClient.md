---
tags:
  - General
Дата создания: 2024-04-20T21:17:00
---
# DelegateCommand


# AsyncDelegateCommand


# Modal windows

https://www.youtube.com/watch?v=M8BAIq0yoy8
https://www.youtube.com/watch?v=umRSp4qB6Tw


# Dialogs

https://intellitect.com/blog/material-design-in-xaml-dialog-host/


```platuml
@startuml
title Алгоритм полной оплаты тренировки
participant User as u
box WPFClient 
participant "Страница тренировок" as tv
participant TrainingViewModel as vm
participant Mediator as m
participant ClientViewModel as cvm
endbox
participant API as a
autonumber
u-> tv: Нажатие на кнопку "Оплатить автоматически"\nв меню конкретной тренировки
tv->vm: Вызвать FullPaymentCommand
vm-> a: Получение типа тренировки по идентификатору
a-> vm: Тип тренировки
vm-> a: Создание списания с баланса клиента\n(BalanceWithdrawing)
a-> vm: Success
vm-> a: Создание связи между списанием и тренировкой\n(TrainingWithdrawing)
a-> vm: Success
vm->tv: Обновить статус тренировки\n(Обновить сумму оплат, изменить свет статуса)
vm->a:Получить данные клиента\nсвязанного с тренировкой
a-> vm: Клиент
vm->m: Вызов события "Баланс клиента обновился" с ClientId
m ->cvm: Обновить отображение баланса клиента
@enduml
```
//www.plantuml.com/plantuml/dpng/XLFDRjD04BxxALOvKOc-G0zHeLAb7Yf5ugUNNhRn2YpPcofPft1BQWI7808SaIWgJ-2c3JLkuhx2x1lnxU34SclA8UlixCmtttpxxVR0oBw9ez0pWGcLeCzqGvTKs15bzfGMWcwfG6Y9zRV0SejDQa_jH60PqHmd6Ru5pIYdp3i6Mj0DZgKsuiL0zOKSYDWxwhqH1vtTdJ1GYDUIMdJEG2a3lQVq7Yv-USlXc66jydbV1ZhGhru6wcI_vwlGvGoZMiw-yWDfUaoZVlANIwssYsAbVP2jPJxfxBbJwSdOz7GS7Qc-5s-sGMTBq7SG_-K4eOpc0gHJ0HsMB5Hk4untjfBC2Me_2XJCi2beKGO_qDoEdNYjGqqpWJY6jvySJW6atAc1HBw1Pj7DSBCzZC3cgvtGDO8fTzgDmx0ZtqQOSwSNHLBxtZ02SOdC7-vMROBc30RkZWkefeqjs0O22LVkX6c_moxd4M0MctWIo4pYfyDfHAdQdoDq3R1qrNoC1XbM3jWf0e8kiBjn4HU77DXaPVlqK3zwAaEfk-eWCA_zlZo14pRM99x5tQuQ31viEBDJX38M77TuPPCxD126_xEcKN0LOB1ou7yfi5NECD2Iwza8QEO6mT_4ZXlbGeUceWI45o1RFSR7O8bHs3ZhF9J0DhKkb6um7RbLtN-PNqgpX87czwHUYyKfgBoa0i3_4APomhSAXljM5aKo0F2vi1FqS3VUeY_LVT-XW5ah7S1xRASjr9UFUC_t8h7PnlDjK9aA0BW-8twffTiVDfUthRGVH-4V