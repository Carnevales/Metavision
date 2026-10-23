# Interface do museu

MuseumExperience, anexado ao Visitante_Inspecao existente, desenha HUD, indicação E, bilheteria, ingresso, pausa e painéis. A escala de referência é 1600 × 1000, ajustada ao tamanho da tela.

As referências dos pontos e das duas folhas existentes do auditório são configuradas por MuseumBuilder. As propriedades de conteúdo ficam nos componentes MuseumPoint da raiz Museu_Interativo_SESI. Não adicione outro controlador ou Canvas para executar essa UI.

Ingressos são fictícios, sem valor comercial, gratuitos e únicos por sessão. Os botões chamam os mesmos métodos utilizados na validação em Play.

Polimento: a escala agora é uniforme com área segura central, sem distorção de tipografia. Aviso AccessDenied é modal e fecha por botão ou Esc. GuideToTicket ativa orientação relativa/distância e encerra na emissão. Consulte References/POLIMENTO.md.
