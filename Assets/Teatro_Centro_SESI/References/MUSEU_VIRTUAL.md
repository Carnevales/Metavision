# Museu virtual — Teatro Firjan SESI Centro

Cena integrada: `Scenes/Teatro_Centro_SESI.unity`. O teatro e o percurso anteriores são reutilizados. Não foram criados outro projeto, outro controlador ou outra câmera de jogador. O componente VisitaCentroSESI existente agora controla mouse livre em primeira pessoa, sensibilidade, suavização, caminhada e colisões; MuseumExperience centraliza ingresso, porta, exposições e interface.

## Uso

Play começa na área externa. WASD caminha, mouse olha, E interage com o ponto mais próximo e Esc fecha uma interface ou alterna o menu de pausa. Na bilheteria à esquerda da entrada, E abre a interface. `Retirar ingresso` emite uma única admissão fictícia por sessão; `Concluir` fecha. Cancelar não emite ingresso. A porta do auditório exige ingresso e outra interação E para validar, animar as duas folhas e liberar o colisor central.

Quatro painéis: sede e cidade; SESI e sua atuação; história do teatro; caixa cênica, iluminação e som. Cada um abre texto breve com fonte e, quando disponível, fotografia do usuário com crédito. Fotografias contemporâneas não são apresentadas como históricas. O cartaz `Luz em cena` está identificado como espetáculo fictício. Não há pagamento, coleta de dados, autenticação ou conexão de rede durante a visita.

## Edificação e organização

Museu_Interativo_SESI agrupa Paredes_Externas_e_Cobertura, Bilheteria, Foyer_e_Exposicao, Entrada_Auditorio, pontos interativos e luz da bilheteria. Os fechamentos novos possuem espessura de 0,30–0,32 m, piso e cobertura. Os panos de vidro continuam restritos a janelas e portas. A cobertura existe e permanece ativa; para inspeção no Editor pode-se ocultar o objeto `Cobertura integral` sem removê-lo.

O conjunto representa o volume de visita e um fechamento simplificado da massa edificada. Não reproduz todos os 14 pavimentos e dois subsolos documentados para a sede real. Esses pavimentos não são acessíveis nesta experiência. Laterais, fundo, bilheteria, foyer, rampa e circulação sem fotos suficientes são interpretação arquitetônica. A bilheteria é uma instalação educativa fictícia, não uma reprodução documentada do balcão real.

As cópias `Scenes/Teatro/Antes_Museu.unity` e `Antes_Museu_Editor.unity` preservam o estado anterior, inclusive a cópia das alterações abertas no Editor no início desta etapa. Elas não são carregadas junto com a cena principal.

## Fontes verificadas

1. Documento fornecido pelo usuário, armazenado em References/Anexo_Riders_2024.pdf:
https://firjan.com.br/data/files/EB/80/20/E2/E48F29104AAA0C29D8284EA8/11%20Anexo%20VIII%20-%20Riders%20Tecnicos_2024.pdf
O início identifica Teatro Firjan SESI Campos; as fotos de hall ali não foram atribuídas ao Centro. A seção Centro possui informações técnicas e desenhos de iluminação e palco nas páginas 18–19. As páginas 20–22 dessa cópia renderizam cabeçalho/rodapé, sem fotografia legível de hall. Renders de conferência foram salvos em References/Centro_Rider_P*.png. As dimensões principais também usam o rider Centro 2025 já armazenado.

2. Time Out, 19/10/2023:
https://www.timeout.com/pt/rio-de-janeiro/teatro/teatro-firjan-sesi-centro
Confirma endereço, inauguração do teatro em 1994 e capacidade de 338 pessoas. A foto da fachada é compatível com a referência do usuário.

3. Publicações do teatro em FindGlocal:
https://www.findglocal.com/BR/Rio-de-Janeiro/514524942268852/Teatro-Firjan-SESI-Centro
Identificação da unidade, espetáculos e menção à bilheteria. Não foi usada para afirmar uma posição arquitetônica exata do balcão.

4. Fonte oficial da sede, Carta da Indústria / Firjan:
https://cartadaindustria.firjan.com.br/publicacoes/trinta-anos-da-firjan-sede
Confirma Centro Empresarial Arthur João Donato, inaugurado em 12/09/1994. Oferece imagens históricas com crédito Firjan; não foram redistribuídas como texturas sem confirmação de licença.

5. História do SESI, Firjan / Memória da Indústria:
https://memoriadaindustria.firjan.com.br/linha-do-tempo/industrias-base/criacao-do-sesi-nova-instituicao-de-apoio-ao-trabalhador
Usada para a criação do SESI em 1946 e sua atuação educativa, cultural e social.

## Assets e licenças

Madeira e granito reutilizam os assets Poly Haven CC0 já importados, com fontes documentadas nas etapas anteriores. Portas existentes, cadeiras, balcão, monitor, teclado, banco de espera e molduras são construídos em Unity. Nenhum asset pago foi adquirido.

Starter Assets FirstPerson foi pesquisado e confirmado gratuito:
https://assetstore.unity.com/packages/essentials/starter-assets-firstperson-updates-in-new-charactercontroller-pa-196525
Não foi importado porque já havia controlador funcional; manteve-se o Input System do projeto. Foram pesquisados modelos gratuitos de monitor; a alternativa própria evitou introduzir conversores, login e dependências para uma peça simples. Nenhum atendente humano 3D foi importado.

## Ferramentas e verificação

Menu `Teatro SESI > Museu > Construir ou atualizar museu` recompõe apenas a raiz do museu, reutiliza as folhas da porta anterior e configura referências do controlador existente. Edições manuais nessa raiz serão substituídas; outros ambientes são preservados. Rodar os geradores antigos de teatro/percurso exige rodar o gerador de museu em seguida para reconfigurar a porta e os pontos.

Menu `Teatro SESI > Museu > Testar fluxo completo` executa em Play o caminho com CharacterController, testa a porta sem ingresso, cancelamento, emissão única, validação, animação, quatro painéis, parede e degraus até o palco. Usa os mesmos métodos de interação e estado do jogo; o teste visual de tecla E e botões é conferido separadamente no Editor. Relatórios: museu-envelope.txt e museu-runtime.txt. Evidências de interface em Lighting/Museu_UI_*.png.

UI é desenhada por MuseumExperience em uma única camada IMGUI adaptável à tela; não depende de Canvas/EventSystem duplicados. UI/README.md descreve a configuração. O sistema não persiste ingresso entre sessões Play.

Verificação manual adicional: E abriu a bilheteria na Game View; os botões emitiram e concluíram o ingresso; Esc abriu a pausa. Registro em museu-ui-manual.txt. Contraste de texto fixado também para estados hover/focus.
