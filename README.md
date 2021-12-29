# CS Logger

Uma Lib criada com fins educacionais, para evidênciar como podemos lidar com processos async, grandes volumes de informação sendo processadas com resiliência e flexibilidade tanto no formato como nos destinos para onde a informação deve ser persistida

## Requisitos
  - Deve ser capaz de gravar os logs em arquivos, enviar para o console e ser extensível para novos "outputs"
  - Deve ser capaz de formatar o log em modo texto, json, xml e ser extensível para formatos customizados
  - O processamento dos logs não deve bloquear ou impedir a thread que chama, o impacto de performance deve ser mínimo na aplicação
  - A lib deve pertir ser totalmente configurável, ou em sua inicialização ou via um arquivo de configuração (json)
  - A lib deve ser capaz de receber requisições paralelas e ser "lock Free", sem perder dados por concorrência
  - A lib deve ser compatível com Linux e Windows
